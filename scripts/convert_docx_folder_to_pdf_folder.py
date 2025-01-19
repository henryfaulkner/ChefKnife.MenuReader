import os
import sys
from docx2pdf import convert

def convert_docx_to_pdf(input_folder, output_folder):
    """
    Converts all .docx files in the input folder to .pdf files in the output folder.

    Parameters:
    input_folder (str): Path to the folder containing .docx files.
    output_folder (str): Path to the folder where .pdf files will be saved.
    """
    # Check if input folder exists
    if not os.path.exists(input_folder):
        print(f"Error: Input folder '{input_folder}' does not exist.")
        sys.exit(1)

    # Create output folder if it doesn't exist
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    # List all .docx files in the input folder
    docx_files = [f for f in os.listdir(input_folder) if f.endswith('.docx')]

    if not docx_files:
        print("No .docx files found in the input folder.")
        return

    print(f"Converting {len(docx_files)} files...")

    for docx_file in docx_files:
        input_path = os.path.join(input_folder, docx_file)
        output_path = os.path.join(output_folder, os.path.splitext(docx_file)[0] + '.pdf')

        try:
            # Convert .docx to .pdf
            convert(input_path, output_path)
            print(f"Converted: {docx_file} -> {os.path.basename(output_path)}")
        except Exception as e:
            print(f"Failed to convert {docx_file}: {e}")

    print("Conversion process completed.")

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python script_name.py <input_folder> <output_folder>")
        sys.exit(1)

    input_folder = sys.argv[1]
    output_folder = sys.argv[2]

    convert_docx_to_pdf(input_folder, output_folder)
