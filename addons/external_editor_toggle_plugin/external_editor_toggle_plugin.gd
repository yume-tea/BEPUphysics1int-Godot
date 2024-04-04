@tool
extends EditorPlugin

var checkbox = CheckBox.new()
var settings = get_editor_interface().get_editor_settings()

func _enter_tree():
    checkbox.text = "Use External Editor"
    checkbox.set_pressed(settings.get("text_editor/external/use_external_editor"))
    checkbox.connect("toggled", Callable(self, "_on_CheckBox_toggled"))
    add_control_to_container(EditorPlugin.CONTAINER_TOOLBAR, checkbox)

func _exit_tree():
    remove_control_from_container(EditorPlugin.CONTAINER_TOOLBAR, checkbox)
    checkbox.free()

func _on_CheckBox_toggled(pressed):
    settings.set("text_editor/external/use_external_editor", pressed)