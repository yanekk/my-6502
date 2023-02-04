while getopts f:m: flag
do
    case "${flag}" in
        f) file_name=${OPTARG%.*};;
        m) memory_map_config="-C ${OPTARG}";;
    esac
done

if [ -z "$file_name" ]; then
    echo "source file not set"
    exit 1
fi

source_file="$file_name.s"
object_file="bin/$file_name.o"
binary_file="bin/$file_name.bin"
map_file="bin/$file_name.bin.lmap"

rm -f "$binary_file"

echo "Assembling $source_file..."
ca65 --verbose -g --cpu 65C02 -I lib -o "$object_file"  --feature string_escapes "$source_file"

if [ $? -ne 0 ]; then 
  echo "Assembly failed." 
  exit 1
fi

echo "Linking $source_file..."
cl65 -t none $memory_map_config -o "$binary_file" -Ln "$map_file" -vm "$object_file"

if [ $? -ne 0 ]; then 
  echo "Linking failed." 
  exit 1
fi

rm -f "$object_file"
if [ -f "$binary_file" ]; then
    echo "Assembled. Output size: $(wc -c $binary_file | awk '{print $1}')"
fi