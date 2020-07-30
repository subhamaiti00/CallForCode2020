import {Component} from "@angular/core";
import {NavController, AlertController, ToastController, MenuController, LoadingController, Events,NavParams} from "ionic-angular";
import {LoginPage} from "../login/login";
import {HomePage} from "../home/home";
import { HttpHeaders, HttpClient } from "@angular/common/http";
import {Observable} from 'Rxjs/rx';
import { Subscription } from "rxjs/Subscription";
import { Storage } from '@ionic/storage';

@Component({
  selector: 'page-register',
  templateUrl: 'register.html'
})
export class RegisterPage {
email:any;pwd:any;name:any;add:any;mob:any;cpwd:any;utype:any;
apiurl:any;deviceinfo:any;
  constructor(public nav: NavController,public loadingCtrl: LoadingController,public toastCtrl: ToastController,
    public storage: Storage,public http: HttpClient,public events: Events,public navParams: NavParams) {
      this.utype=this.navParams.get('utype');
      this.storage.get('apiurl').then((val) => {
        this.apiurl=val;
        console.log("apiurl="+this.apiurl);      
     
         storage.get('deviceinfo').then((val) => {
          this.deviceinfo=val;
         });        
      });
  }

  // register and go to home page
  register1() {
    //this.nav.setRoot(HomePage);
    let loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    var datax={utype:this.utype,uid:this.email,pwd:this.pwd,add:this.add,name:this.name,mob:this.mob};
    this.http.post('https://callforcodeapi-impressive-wolf-lg.eu-gb.mybluemix.net/api/Users/Register',datax).subscribe(data1 => {
    //this.http.post(this.apiurl+'/S1/service.ashx?method=register',JSON.stringify(datax)).subscribe(data1 => {
      loader.dismiss();
      //alert(data1);
    if(data1==null || data1=="")
    {      
      this.presentToast("Invalid Data/Email alrady Exists");
    }
    else
    {
      //alert("Registration Successful.Please login.");
      let toast = this.toastCtrl.create({
        message: 'Registration Successful.Please login.',
        duration: 3000,
        position: 'top',
        cssClass: 'dark-trans',
        closeButtonText: 'OK',
        showCloseButton: true
      });
      toast.present();
      this.nav.setRoot(LoginPage);
    }
  });
  }

  // go to login page
  login() {
    this.nav.setRoot(LoginPage);
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
