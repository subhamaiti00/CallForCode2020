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
import { ViewChild } from '@angular/core';
import { Content } from 'ionic-angular';

@Component({
  selector: 'page-chatbot',
  templateUrl: 'chatbot.html',
  styleUrls: ['/home/chatbot.scss']
})
export class chatbot {
email:any;pwd:any;name:any;add:any;mob:any;cpwd:any;buttonClicked:any;
apiurl:any;deviceinfo:any;data:any;editorMsg:any;
@ViewChild(Content) contentArea: Content;
  constructor(public nav: NavController,public loadingCtrl: LoadingController,public toastCtrl: ToastController,
    public storage: Storage,public http: HttpClient,public events: Events) {

      this.data=[];
      this.storage.get('apiurl').then((val) => {
        this.apiurl=val;
        console.log("apiurl="+this.apiurl);      
     
         storage.get('deviceinfo').then((val) => {
          this.deviceinfo=val;
         });        
      });
  }

  sendMsg()
  {
    let modelData = {
        type: 'q',
        result: ''+this.editorMsg+'',
        status: 'pending',
        userAvatar: 'http://callforcodeapi.abahan.com/av.jpg'
    }; 
    this.data.push(modelData);
    const params = {};
    const headers = {};
    this.http.get('https://callforcodeapi-impressive-wolf-lg.eu-gb.mybluemix.net/api/Watson/'+this.editorMsg,{responseType: 'text'}).subscribe(data1 => {
      this.data.forEach((id1:any,text1:any) => {
        id1.status='done';
      }); 
      modelData = {
          type: 'a',
          result: data1,
          status: 'done',
          userAvatar: 'http://callforcodeapi.abahan.com/withu.jpg'
      }; 
    this.data.push(modelData);
    this.editorMsg='';
    this.contentArea.scrollToBottom();
    });
  
  
  }

}
