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
import { sehome } from "./sehome";

@Component({
  selector: 'page-vieworderSE',
  templateUrl: 'vieworderSE.html'
})

export class vieworderSE {
  // search condition
  public search = {
    name: "Rio de Janeiro, Brazil",
    date: new Date().toISOString()
  }
  public trips: any;otp:any;show:any;sts:any;
  public Products: any;apiurl:any;catid:any;uid:any;cartval:number;
  public OrderID: any;UName:any;Date:any;OrderValue:any;address:any;city:any;pin:any;rem:any;toDate:any;Mobile:any;
  SEList: any = [
    {ID: 1, UName: "test1", ischecked: false}
 ]

selectedArray :any = [];
  constructor(private storage: Storage, public nav: NavController, public popoverCtrl: PopoverController,
    public navParams: NavParams,public http: HttpClient,public loadingCtrl: LoadingController,public toastCtrl: ToastController) 
    {
      this.show=false;
      this.OrderID=this.navParams.get('ID');
      this.UName=this.navParams.get('UName');
      this.Date=this.navParams.get('Date');
      this.OrderValue=this.navParams.get('OrderValue');
      this.city=this.navParams.get('city');
      this.pin=this.navParams.get('pin');
      this.address=this.navParams.get('address');

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
    this.http.get(this.apiurl+'/S1/service.ashx?method=vieworderAdmin&id='+this.OrderID).subscribe(data1=> {
      loader.dismiss();
      //alert(data1);
    if(data1==null || data1=="")
    {      
     // this.presentToast("No Record(s)");
    }
    else
    {
      this.Products=data1;  
      this.cartval=0;      
      for (let item of this.Products){
        this.cartval+=Number(item.Price);
        console.log(item.Price);
      }
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

   changeStatus(val)
   {
     this.sts=val;
     if(val=="Completed")
     {
          let loader = this.loadingCtrl.create({
            content: "Please wait..."
          });
          loader.present();
        this.http.get(this.apiurl+'/S1/service.ashx?method=otpgen&orderid='+this.OrderID).subscribe(data1=> {
          loader.dismiss();
          //alert(data1);
        if(data1==null || data1=="")
        {      
          this.presentToast("No Record(s)");
        }
        else
        {
          if(data1=="1")
          {
            this.show=true;
            //alert("")
          }
        }
      });
          
    }
     else if(val=="InProgress")
      this.show=false;      
   }

   UpdateOrder()
   {
    /* alert(this.checked)*/    
     if(this.sts=="Completed")
     {
      if(this.otp=="")
      {
        alert("Please enter otp.");
        return;
      }
     }
    
     let loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    loader.present();
      this.http.get(this.apiurl+'/S1/service.ashx?method=updateorder&sts='+this.sts+'&orderid='+this.OrderID+'&otp='+this.otp
                   +'&rem='+this.rem+'&uid='+this.uid
      ).subscribe(data1=> {
        loader.dismiss();
      if(data1==null || data1=="")
      {      
       this.presentToast("OTP mismatched.");
      }
      else
      {
        if(data1=="1")
        { 
          alert("Request updated successfully.");
          this.nav.setRoot(sehome);
        }
      }
    });
   }
}

//
