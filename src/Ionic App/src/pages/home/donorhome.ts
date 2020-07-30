import {Component} from "@angular/core";
import {NavController, AlertController, ToastController, MenuController, LoadingController, Events} from "ionic-angular";
import {LoginPage} from "../login/login";
import {HomePage} from "../home/home";
import { HttpHeaders, HttpClient } from "@angular/common/http";
import {Observable} from 'Rxjs/rx';
import { Subscription } from "rxjs/Subscription";
import { Storage } from '@ionic/storage';
import { RegisterPage } from "../register/register";
import { myorderPage } from "../orders/myorder";
import { viewcartPage } from "../viewcart/viewcart";
import { vieworder } from "../orders/vieworder";
import { neworders } from "../Admin/neworders";
import { DonorHistory } from "../Admin/DonorHistory";

@Component({
  selector: 'page-donorhome',
  templateUrl: 'donorhome.html'
})
export class DonorHome {
email:any;pwd:any;name:any;add:any;mob:any;cpwd:any;
apiurl:any;deviceinfo:any;
  constructor(public nav: NavController,public loadingCtrl: LoadingController,public toastCtrl: ToastController,
    public storage: Storage,public http: HttpClient,public events: Events) {
      this.storage.get('apiurl').then((val) => {
        this.apiurl=val;
        console.log("apiurl="+this.apiurl);      
     
         storage.get('deviceinfo').then((val) => {
          this.deviceinfo=val;
         });        
      });
  }

  // register and go to home page
  openrequests() {
   this.nav.push(neworders);
  }
  mydonations() {
    this.nav.push(DonorHistory);
   }
    
  presentToast(msg) {
    let toast = this.toastCtrl.create({
      message: msg,
      duration: 3000,
      position: 'bottom'
    });
  
    toast.onDidDismiss(() => {
      console.log('Dismissed toast');
    });
  
    toast.present();
  }
}
