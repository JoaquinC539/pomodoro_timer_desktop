import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import  {invoke, isTauri} from '@tauri-apps/api/core'

@Injectable({
  providedIn: 'root'
})
export class AudioService {

  constructor(@Inject(PLATFORM_ID) private platform_id:Object) { }

  public callTestCommand(){
    invoke("my_test_command")
  }
  public async playSound(filename:string):Promise<void>{    
    
    if(await !isTauri()){
      try {
        (new Audio("/sounds/"+filename)).play().catch(e => console.warn('Web audio failed:', e));
      } catch (e) {
        console.error('Web audio methods failed:', e);
      }      
      
    }else{
      invoke("play_sound",{filename})
    .then(()=>console.log("Playback triggered"))
    .catch(console.error);
    }
    
  }

}
