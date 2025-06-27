use std::{  path::PathBuf, thread::spawn};

use tauri::{path::{ BaseDirectory}, Manager};

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
  tauri::Builder::default()
    .setup(|app| {
      if cfg!(debug_assertions) {
        app.handle().plugin(
          tauri_plugin_log::Builder::default()
            .level(log::LevelFilter::Info)
            .build(),
        )?;
      }
      Ok(())
    })
    // .invoke_handler(tauri::generate_handler![my_test_command])
    // .invoke_handler(tauri::generate_handler![play_sound])
    // .invoke_handler(tauri::generate_handler![get_os_info])
    .invoke_handler(tauri::generate_handler![
      my_test_command,
      play_sound,
      get_os_info
    ])
    .run(tauri::generate_context!())
    .expect("error while running tauri application");
}
#[tauri::command]
fn my_test_command(){
  println!("I was invoked by js");
}

#[tauri::command]
fn get_os_info()->String{
  let os = std::env::consts::OS;
  os.to_string()
}

#[tauri::command]
fn play_sound(handle:tauri::AppHandle,filename: String)-> Result<(), String>{
    // let path = handle.path().resolve(format!("sounds/{}",filename), BaseDirectory::Resource);
  
    
    
    
    // let mut path = std::env::current_dir()    
    // .map_err(|e| format!("Could not get current dir: {}",e))?;  
    // path.pop();  
    // path.push("public");
    // path.push("sounds");
    // path.push(&filename);
    
    let path = handle.path().resolve(format!("sounds/{}",filename), BaseDirectory::Resource)
    .map_err(|e| format!("Could not get current dir: {}",e))?; 

    
    if !path.exists(){
      return Err(format!("File not found: {:?}",path));
    }
    play_with_platform_specific(&path)

  
}

fn play_with_platform_specific(path: &PathBuf) -> Result<(),String>{
    let path = path.clone();

    spawn(move || {
      #[cfg(target_os = "linux")]
      {      
        let _ = std::process::Command::new("paplay")
        .arg(path)
        .spawn()
        .and_then(|mut child|child.wait());
        // std :: process::Command::new("paplay")
        // .arg(path)
        // .spawn()
        // .map_err(|e| format!("Linux playback failed: {}",e))?
        // .wait()
        // .map_err(|e| format!("Playback error: {}",e))?;
      }
      #[cfg(target_os = "windows")]
      {
          let _ = std::process::Command::new("cmd")
                .args(&["/C", "start", "", "/MIN", path.to_str().unwrap()])
                .spawn()
                .and_then(|mut child| child.wait());
      }

      #[cfg(target_os = "macos")]
      {
          let _ = std::process::Command::new("afplay")
                .arg(path)
                .spawn()
                .and_then(|mut child| child.wait());
      }
    });
    
    Ok(())

  }



