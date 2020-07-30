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
import { placeorderPage } from "../orders/placeorder";
import { vieworder } from "./vieworder";

@Component({
  selector: 'page-ordersuccess',
  templateUrl: 'ordersuccess.html'
})

export class ordersuccess {
  // search condition
  public search = {
    name: "Rio de Janeiro, Brazil",
    date: new Date().toISOString()
  }
  public trips: any;
  public Products: any;apiurl:any;catid:any;uid:any;cartval:number;payid:any;

  constructor(private storage: Storage, public nav: NavController, public popoverCtrl: PopoverController,
    public navParams: NavParams,public http: HttpClient,public loadingCtrl: LoadingController,public toastCtrl: ToastController) 
    {
     
      this.payid=this.navParams.get('payid');
      this.storage.get('apiurl').then((val) => {
        this.apiurl=val;
        this.storage.get('uid').then((val1) => {
        this.uid=val1;

    });
  });
  this.storage.get('uid').then((val) => {
    this.uid=val;
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
 
  }

  // go to result page
  doSearch() {
    this.nav.push(TripsPage);
  }

  // choose place
  choosePlace(from) {
    this.nav.push(SearchLocationPage, from);
  }

  // to go account page
  goToAccount() {
    this.nav.push(SettingsPage);
  }
  fnViewOrder()
  {
    this.nav.push(vieworder);
  }
  presentNotifications(myEvent) {
    console.log(myEvent);
    let popover = this.popoverCtrl.create(NotificationsPage);
    popover.present({
      ev: myEvent
    });
  }
}

 

//
