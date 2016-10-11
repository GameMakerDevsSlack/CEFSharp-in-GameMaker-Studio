///scr_cef_kill()

var bat = file_text_open_write("kill.bat");
file_text_write_string(bat,"@echo off");
file_text_writeln(bat);
file_text_write_string(bat,"taskkill /IM cefsharp.exe /F");
file_text_close(bat);
shell_execute(game_save_path + "\kill.bat","")
