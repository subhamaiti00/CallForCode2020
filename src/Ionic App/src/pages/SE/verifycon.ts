import {Component} from "@angular/core";
import {NavController, PopoverController, LoadingController, ToastController,NavParams} from "ionic-angular";
import {Storage} from '@ionic/storage';

import {NotificationsPage} from "../notifications/notifications";
import {SettingsPage} from "../settings/settings";
import {TripsPage} from "../trips/trips";
import {SearchLocationPage} from "../search-location/search-location";
import {TripService} from "../../services/trip-service";
import {TripDetailPage} from "../trip-detail/trip-detail";
import { HttpClient } from "@angular/common/http";
import { neworders } from "./neworders";

@Component({
  selector: 'page-verifycon',
  templateUrl: 'verifycon.html'
})

export class verifycon {
  // search condition
  public search = {
    name: "",
    date: new Date().toISOString()
  }
  public trips: any;
  public consumers: any;apiurl:any;catid:any;uid:any;cartval:number;
  public OrderID: any;UName:any;Date:any;OrderValue:any;address:any;city:any;pin:any;fromDate:any;toDate:any;
  SEList: any = []

selectedArray :any = [];
  constructor(private storage: Storage, public nav: NavController, public popoverCtrl: PopoverController,
    public navParams: NavParams,public http: HttpClient,public loadingCtrl: LoadingController,public toastCtrl: ToastController) 
    {
     
      // this.OrderID=this.navParams.get('ID');
      // this.UName=this.navParams.get('UName');
      // this.Date=this.navParams.get('Date');
      // this.OrderValue=this.navParams.get('OrderValue');
      // this.city=this.navParams.get('city');
      // this.pin=this.navParams.get('pin');
      // this.address=this.navParams.get('address');

      this.storage.get('apiurl').then((val) => {
        this.apiurl=val;
        this.storage.get('uid').then((val1) => {
        this.uid=val1;
        this.Loaddata();
    });
  });
  this.storage.get('uid').then((val) => {
    this.uid=val;
  });
  }
  Loaddata()
  {
    let loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    loader.present();
  
    this.http.get(this.apiurl+'/S1/service.ashx?method=pendingverify&uid='+this.uid).subscribe(data1=> {
      loader.dismiss();
      //alert(data1);
    if(data1==null || data1=="")
    {      
     // this.presentToast("No Record(s)");
    }
    else
    {
      this.consumers=data1;  
  
    }
  });
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
  ionViewWillEnter() {
    // this.search.pickup = "Rio de Janeiro, Brazil";
    // this.search.dropOff = "Same as pickup";
    this.storage.get('pickup').then((val) => {
      //this.nav.push(TripsPage);
    }).catch((err) => {
      console.log(err)
    });
  }

  // go to result page
  doSearch() {
    this.nav.push(TripsPage);
  }
  approve(id)
  {
    let loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    loader.present();
    this.http.get(this.apiurl+'/S1/service.ashx?method=approve_reject&action=a&id='+id).subscribe(data1 => {
      loader.dismiss();
      //alert(data1);
    if(data1==null || data1=="")
    {      
      this.presentToast("Unable to update.");
    }
    else if(data1=="1")
    {
      this.Loaddata();
    let toast = this.toastCtrl.create({
      message: 'Approved.',
      duration: 2000,
      position: 'bottom',
      cssClass: 'dark-trans',
      closeButtonText: 'OK',
      showCloseButton: true
    });
    toast.present();
  }
  else{
  }
  });
  }
  reject(id)
  {
    let loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    loader.present();
    this.http.get(this.apiurl+'/S1/service.ashx?method=approve_reject&action=r&id='+id).subscribe(data1 => {
      loader.dismiss();
      //alert(data1);
    if(data1==null || data1=="")
    {      
      this.presentToast("Unable to update.");
    }
    else if(data1=="1")
    {
      this.Loaddata();
    let toast = this.toastCtrl.create({
      message: 'Rejected.',
      duration: 2000,
      position: 'bottom',
      cssClass: 'dark-trans',
      closeButtonText: 'OK',
      showCloseButton: true
    });
    toast.present();
  }
  else{
  }
  });
  }
  // choose place
  

  presentNotifications(myEvent) {
    console.log(myEvent);
    let popover = this.popoverCtrl.create(NotificationsPage);
    popover.present({
      ev: myEvent
    });
  }
  checked = [];
}

//
