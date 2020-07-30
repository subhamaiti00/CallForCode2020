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
import { viewcartPage } from "../viewcart/viewcart";
import { placeorderPage } from "../orders/placeorder";
import { ProductsPage } from "../products/products";

@Component({
  selector: 'page-subcat',
  templateUrl: 'subcat.html'
})

export class subcatPage {
  // search condition
  public search = {
    name: "Rio de Janeiro, Brazil",
    date: new Date().toISOString()
  }
  public trips: any;
  public Products: any;apiurl:any;catid:any;uid:any;ctname:any;subcats:any
  public num:any=[{val:''}]
  constructor(private storage: Storage, public nav: NavController, public popoverCtrl: PopoverController,
    public navParams: NavParams,public http: HttpClient,public loadingCtrl: LoadingController,public toastCtrl: ToastController) 
    {
      let loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
     
      this.catid=this.navParams.get('id');
      this.ctname=this.navParams.get('name');
      this.storage.get('apiurl').then((val) => {
      this.apiurl=val;
      this.http.get(this.apiurl+'/S1/service.ashx?method=SubCategories&catid='+this.catid).subscribe(data1 => {
        loader.dismiss();
        //alert(data1);
      if(data1==null || data1=="")
      {      
        this.presentToast("No Record(s)");
      }
      else
      {
        this.subcats=data1;
      }
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
  fnAddToCart(id,val) {
    //alert(val);
      //this.nav.push(ProductsPage, {id: id});
      let loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
      this.http.get(this.apiurl+'/S1/service.ashx?method=addtocart&uid='+this.uid+'&pid='+id+'&qty='+val).subscribe(data1 => {
        loader.dismiss();
        //alert(data1);
      if(data1==null || data1=="")
      {      
        this.presentToast("Invalid Data/Email Exists");
      }
      else if(data1=="1")
      {
      let toast = this.toastCtrl.create({
        message: 'Added to cart.',
        duration: 3000,
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
  fnOrderNow(id) {
    this.nav.push(placeorderPage, {From: "",Pid:id});
  }
  viewcart()
  {
    //this.nav.setRoot(viewcartPage);
    this.nav.push(viewcartPage);
  }
  changeAmt(val)
  {
    
  }
  ProductList(id,name)
  {
    this.nav.push(ProductsPage, {id: id,name:name});
  }
}

//
