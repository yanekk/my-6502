import os

from pathlib import Path
from approvaltests import verify
from ..piskel_to_binary import ImageData, parse_data, image_data_to_assembly_code, image_file_to_assembly_file

def test_single_frame():
    # arrange
    data_file = Path(__file__).parent / 'data/single_frame.piskel'
    expected_image = ImageData(8, 8, [
        [0, 0, 1, 1, 1, 1, 0, 0],
        [0, 1, 0, 0, 0, 0, 1, 0],
        [1, 0, 1, 0, 0, 1, 0, 1],
        [1, 0, 0, 0, 0, 0, 0, 1],
        [1, 0, 0, 0, 0, 0, 0, 1],
        [1, 0, 0, 1, 1, 0, 0, 1],
        [0, 1, 0, 0, 0, 0, 1, 0],
        [0, 0, 1, 1, 1, 1, 0, 0],
        # a smiley
    ])

    # act
    data = parse_data(data_file)

    # assert
    assert len(data) == 1
    assert data[0] == expected_image
    
def test_multiple_frames():
    # arrange
    data_file = Path(__file__).parent / 'data/multiple_frames.piskel'
    expected_images = [
        ImageData(2, 2, [
            [1, 0],
            [0, 0]
        ]),
        ImageData(2, 2, [
            [0, 1],
            [0, 0]
        ]),
        ImageData(2, 2, [
            [0, 0],
            [1, 0]
        ]),
        ImageData(2, 2, [
            [0, 0],
            [0, 1]
        ])]

    # act
    data = parse_data(data_file)

    # assert
    assert data == expected_images

def test_image_data_to_assembly_code():
    # arrange
    input_image = ImageData(8, 8, [
        [0, 0, 1, 1, 1, 1, 0, 0],
        [0, 1, 0, 0, 0, 0, 1, 0],
        [1, 0, 1, 0, 0, 1, 0, 1],
        [1, 0, 0, 0, 0, 0, 0, 1],
        [1, 0, 0, 0, 0, 0, 0, 1],
        [1, 0, 0, 1, 1, 0, 0, 1],
        [0, 1, 0, 0, 0, 0, 1, 0],
        [0, 0, 1, 1, 1, 1, 0, 0],
        # a smiley
    ])

    # act
    actual_assembly_code = image_data_to_assembly_code(input_image)

    # assert
    assert actual_assembly_code == '.byte %00111100, %01000010, %10100001, %10000101, %10000101, %10100001, %01000010, %00111100'

def test_single_frame_image_file_to_assembly_file():
    # arrange
    data_file_path = Path(__file__).parent / 'data/single_frame.piskel'

    # act
    image_file_to_assembly_file(data_file_path)

    # assert
    assembly_code_file_path = data_file_path.with_suffix('.s')
    assert os.path.exists(assembly_code_file_path)
    with open(assembly_code_file_path, 'rt') as assembly_code_file:
        assembly_code = assembly_code_file.read()
    verify(assembly_code)

    # cleanup
    os.remove(assembly_code_file_path)

def test_multiple_frame_image_file_to_assembly_file():
    # arrange
    data_file_path = Path(__file__).parent / 'data/moving_square.piskel'

    # act
    image_file_to_assembly_file(data_file_path)

    # assert
    assembly_code_file_path = data_file_path.with_suffix('.s')
    assert os.path.exists(assembly_code_file_path)
    with open(assembly_code_file_path, 'rt') as assembly_code_file:
        assembly_code = assembly_code_file.read()
    verify(assembly_code)

    # cleanup
    os.remove(assembly_code_file_path)