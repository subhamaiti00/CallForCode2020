import {Component} from "@angular/core";
import {NavController, PopoverController, LoadingController, ToastController} from "ionic-angular";
import {Storage} from '@ionic/storage';

import {NotificationsPage} from "../notifications/notifications";
import {SettingsPage} from "../settings/settings";
import {TripsPage} from "../trips/trips";
import {SearchLocationPage} from "../search-location/search-location";
import {TripService} from "../../services/trip-service";
import {TripDetailPage} from "../trip-detail/trip-detail";
import { HttpClient } from "@angular/common/http";
import { ProductsPage } from "../products/products";
import { subcatPage } from "./subcat";

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})

export class HomePage {
  // search condition
  public search = {
    name: "Rio de Janeiro, Brazil",
    date: new Date().toISOString()
  }
  public trips: any;
  public Categories: any;apiurl:any;

  constructor(private storage: Storage, public nav: NavController, public popoverCtrl: PopoverController,
    public tripService: TripService,public http: HttpClient,public loadingCtrl: LoadingController,public toastCtrl: ToastController) 
    {
      let loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
      this.storage.get('apiurl').then((val) => {
        this.apiurl=val;
      this.http.get('https://callforcodeapi-impressive-wolf-lg.eu-gb.mybluemix.net/api/Category/Category').subscribe(data1 => {
        loader.dismiss();
        //alert(data1);
      if(data1==null || data1=="")
      {      
        this.presentToast("No Record(s)");
      }
      else
      {
        this.Categories=data1;
      }
    });
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

  // choose place
  choosePlace(from) {
    this.nav.push(SearchLocationPage, from);
  }

  // to go account page
  goToAccount() {
    this.nav.push(SettingsPage);
  }

  presentNotifications(myEvent) {
    console.log(myEvent);
    let popover = this.popoverCtrl.create(NotificationsPage);
    popover.present({
      ev: myEvent
    });
  }
  subcatList(id,name) {
   // this.nav.push(ProductsPage, {id: id,name:name});
   this.nav.push(subcatPage, {id: id,name:name});
  }
}

//
