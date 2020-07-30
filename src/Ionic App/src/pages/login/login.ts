import {Component} from "@angular/core";
import {NavController, AlertController, ToastController, MenuController, LoadingController, Events} from "ionic-angular";
import {HomePage} from "../home/home";
import {RegisterPage} from "../register/register";
import { HttpHeaders, HttpClient } from "@angular/common/http";
import {Observable} from 'Rxjs/rx';
import { Subscription } from "rxjs/Subscription";
import { Storage } from '@ionic/storage';
import { neworders } from "../Admin/neworders";
import { sehome } from "../SE/sehome";
import { HomeRegisterPage } from "../homeregister/homeregister";
import { ConsumerHome } from "../home/consumerhome";
@Component({
  selector: 'page-login',
  templateUrl: 'login.html'
})
export class LoginPage {
  apiurl:any;email:any;pwd:any;role:any;
  constructor(public nav: NavController, public forgotCtrl: AlertController, public menu: MenuController, public toastCtrl: ToastController,
    public loadingCtrl: LoadingController,public storage: Storage,public http: HttpClient,public events: Events) {
    //this.apiurl="http://localhost:16653/";
    this.apiurl="http://callforcodeapi.abahan.com/";
    this.storage.set('apiurl',this.apiurl);
    this.storage.get('role').then((val_e) => { 
      this.role=val_e;
      if(this.role!=null)
      {
        //alert(this.role);
        this.storage.get('name').then((val_name) => {
        this.events.publish('loggedin', this.role,  Date.now(),val_name);
        if(this.role=="c")
        this.nav.setRoot(ConsumerHome);
        else if(this.role=="d")
        this.nav.setRoot(neworders);
        else
        {
          this.nav.setRoot(sehome);
        }
        
      });
      }
    });
    this.menu.swipeEnable(false);
  }

  // go to register page
  register() {
    this.nav.setRoot(HomeRegisterPage); 
  }

  // login and go to home page
  login() {
    //this.nav.setRoot(HomePage);
    
    let loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    loader.present();
    let data={uid:this.email,pwd:this.pwd};
    this.http.post('https://callforcodeapi-impressive-wolf-lg.eu-gb.mybluemix.net/api/Users/Login?uid='+this.email+'&pwd='+this.pwd,'').subscribe(data1 => {
     loader.dismiss();
    if(data1==null || data1=="")
    {      
      this.presentToast("Invalid Pin");
    }
    else
    {
      const josondata=JSON.parse(JSON.stringify(data1));
     
      this.events.publish('loggedin', josondata.role,  Date.now(),josondata.name);
     // this.events.publish('loggedinuid',josondata.uid, Date.now());
      this.storage.clear();    
      this.storage.set('apiurl',this.apiurl);
      this.storage.set('uid',josondata.uid);
      this.storage.set('role',josondata.role);
      this.storage.set('name',josondata.name);
      this.storage.set('email',josondata.email);
      this.storage.set('mob',josondata.mob);
      if(josondata.role=="c")
      this.nav.setRoot(ConsumerHome);
      else if(josondata.role=="d")
      this.nav.setRoot(neworders);
      else
      {
        this.nav.setRoot(sehome);
      }
      
    //this.startTracking(josondata.uid);
  }
  });
  }

  forgotPass() {
    let forgot = this.forgotCtrl.create({
      title: 'Forgot Password?',
      message: "Enter you email address to send a reset link password.",
      inputs: [
        {
          name: 'email',
          placeholder: 'Email',
          type: 'email'
        },
      ],
      buttons: [
        {
          text: 'Cancel',
          handler: data => {
            console.log('Cancel clicked');
          }
        },
        {
          text: 'Send',
          handler: data => {
            console.log('Send clicked');
            let toast = this.toastCtrl.create({
              message: 'Email was sended successfully',
              duration: 3000,
              position: 'top',
              cssClass: 'dark-trans',
              closeButtonText: 'OK',
              showCloseButton: true
            });
            toast.present();
          }
        }
      ]
    });
    forgot.present();
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
