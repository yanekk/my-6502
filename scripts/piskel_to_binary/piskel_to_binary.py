import base64
from collections import namedtuple
import io
import os
from pathlib import Path
import sys
from PIL import Image
import json
from approvaltests import verify

ImageData = namedtuple('ImageData', 'width height pixels')

def parse_data(piskel_file_path: Path):
    with open(piskel_file_path, 'rt') as piskel_file:
        data = json.load(piskel_file)
    
    height = data['piskel']['height']
    width = data['piskel']['width']

    layers = []
    for layer_str in data['piskel']['layers']:
        layer = json.loads(layer_str)
        for chunk in layer['chunks']:
            image = _chunk_data_as_image(chunk)
            for frame_index in range(len(chunk['layout'])):
                frame_data = []
                for y in range(height):
                    row = []
                    for x in range(frame_index * width, frame_index * width + width):
                        pixel = image.getpixel((x, y))[3]
                        row.append(1 if pixel > 0 else 0)
                    frame_data.append(row)

                layers.append(ImageData(width, height, frame_data))

    return layers

def _chunk_data_as_image(chunk):
    raw_data = chunk['base64PNG'][len('data:image/png;base64,'):]
    buffer = io.BytesIO(base64.b64decode(raw_data))
    return Image.open(buffer)

def image_data_to_assembly_code(image_data: ImageData):
    bytes = []
    current_byte = '%'
    for x in range(image_data.width):
        for y in range(image_data.height):
            current_byte += str(image_data.pixels[y][x])
        bytes.append(current_byte)
        current_byte = '%'
    return '.byte ' + ', '.join(bytes)

def image_file_to_assembly_file(image_file_path: Path):
    output_file_path = image_file_path.with_suffix('.s')
    name = output_file_path.stem
    with open(output_file_path, 'wt') as output_file:
        output_file.write(f'{name}:\n')
        layers = parse_data(image_file_path)
        for idx, layer in enumerate(layers):
            output_file.write(f'{name}_{idx}: {image_data_to_assembly_code(layer)}\n')

def test_image_file_to_assembly_file():
    # arrange
    data_file_path = Path(__file__).parent / 'smile.piskel'

    # act
    image_file_to_assembly_file(data_file_path)

    # assert
    assembly_code_file_path = data_file_path.with_suffix('.s')
    assert os.path.exists(assembly_code_file_path)
    with open(assembly_code_file_path, 'rt') as assembly_code_file:
        assembly_code = assembly_code_file.read()
    verify(assembly_code)

if(__name__ == "__main__"):
    image_file_to_assembly_file(Path(sys.argv[1]))