import os

# The root directory to search in
root_dir = "./"

# The subdirectories to search in
subdirs_to_search = ["ScriptableObjects/Builders/Concretes"]

# The subdirectory to create the new files in
new_files_subdir = "Editor/Drawers/ExplicitType"

# The content to write to each new file
new_file_content = """
using UnityEditor;

[CustomPropertyDrawer(typeof([name]))]
public class [name]Drawer : ThingBuilderDrawer<[name], [builtname]> {}
"""

# A list to hold the names of the files we find
file_names = []

# Loop through each subdirectory to search in
for subdir in subdirs_to_search:
    # Find all .cs files in this subdirectory
    for root, dirs, files in os.walk(os.path.join(root_dir, subdir)):
        for file in files:
            if file.endswith(".cs"):
                # Save the file name (without the .cs extension)
                file_names.append(os.path.splitext(file)[0])

print(file_names)

# Loop through each saved file name and create a new file
for i, name in enumerate(file_names):
    new_file_name = name + "Drawer.cs"
    new_file_path = os.path.join(root_dir, new_files_subdir, new_file_name)
    new_file_path = new_file_path.replace("\\", "/")
    # Replace placeholders in the new file content with the actual name
    actual_file_content = new_file_content.replace("[name]", name)
    builtObjectName = name.replace("Builder", "")
    print("Drawer for object " + builtObjectName)
    actual_file_content = actual_file_content.replace("[builtname]", builtObjectName)
    # Create the directory if it doesn't exist
    os.makedirs(os.path.dirname(new_file_path), exist_ok=True)
    # Create the new file
    with open(new_file_path, "w") as f:
        f.write(actual_file_content)
    print(f"Created file: {new_file_path}")

print("Done.")
