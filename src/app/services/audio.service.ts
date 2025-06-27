
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import  { invoke , isTauri} from '@tauri-apps/api/core'



@Injectable({
  providedIn: 'root'
})
export class AudioService {
  os:string="";

  constructor(@Inject(PLATFORM_ID) private platform_id:Object) { }

  public callTestCommand(){
    invoke("my_test_command")
  }
  public async playSound(filename:string):Promise<void>{ 
    const os = await this.getOs()
    console.log(os)

    if(await isTauri() && os==="linux"){
      invoke("play_sound",{filename})
      .then(()=>console.log("Playback triggered"))
      .catch(console.error)
    }else{
      try {
        (new Audio("/sounds/"+filename)).play().catch(e => console.warn('Web audio failed:', e));
      } catch (e) {
        console.error('Web audio methods failed:', e);
      } 
    }
    
    
  }
  public async getOs():Promise<string>{
    if(this.os===""){
      this.os=await invoke<string>("get_os_info")
    }
    return this.os;

  }

}
