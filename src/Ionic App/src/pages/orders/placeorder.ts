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
import { ordersuccess } from "./ordersuccess";

@Component({
  selector: 'page-placeorder',
  templateUrl: 'placeorder.html'
})

export class placeorderPage {
  // search condition
  public search = {
    name: "Rio de Janeiro, Brazil",
    date: new Date().toISOString()
  }
  public trips: any;
  public Products: any;apiurl:any;From:any;uid:any;cartval:number;ordertext:any;name:any;email:any;mob:any;
  public address1:any;city:any;pin:any;landmark:any;PaymentType:any;hide:any;PayAmtType:any;Payableamt:any;pid:any

  constructor(private storage: Storage, public nav: NavController, public popoverCtrl: PopoverController,
    public navParams: NavParams,public http: HttpClient,public loadingCtrl: LoadingController,public toastCtrl: ToastController) 
    {
     this.hide=true;
     this.PaymentType="cash";
     this.PayAmtType="full";
     this.ordertext="Confirm Request";
      this.From=this.navParams.get('From');
      this.storage.get('name').then((val_n) => {
        this.name=val_n;
      });
      this.storage.get('email').then((val_e) => {
        this.email=val_e;
      });
      this.storage.get('mob').then((val_m) => {
        this.mob=val_m;
      });
      this.storage.get('apiurl').then((val) => {
        this.apiurl=val;
        this.storage.get('uid').then((val1) => {
        this.uid=val1;
        if(this.From=="Cart"){
          this.pid="0";
        this.Loaddata(0);
      }
        else
        {
          this.pid=this.navParams.get('Pid');
          
          this.Loaddata(this.pid);
        }
    });
  });
  this.storage.get('uid').then((val) => {
    this.uid=val;
  });
  }
  Loaddata(id)
  {
    let loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    loader.present();
    this.http.get(this.apiurl+'/S1/service.ashx?method=getproducts&pid='+id+'&uid='+this.uid).subscribe(data1=> {
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
      this.Payableamt=this.cartval;
      //this.ordertext="Pay ₹ "+this.cartval.toString()+".00";
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
  changeType(value) {    
    this.PaymentType=value;
    if(value=="online")
    {
      this.hide=true;
      //this.ordertext="Pay ₹ "+this.cartval.toString()+".00";
    }
    else{
      this.hide=false;
      this.ordertext="Confirm Order";
    }
  }
  
  changePayAmt(value) {
    this.PayAmtType=value;
    if(value=="half")
    {
     this.Payableamt=Number(this.cartval)/2;
    }
    else
    {
      this.Payableamt=this.cartval;
    }
    //this.ordertext="Pay ₹ "+this.Payableamt.toString()+".00";
   }
  fnPlaceOrder(){
    //alert(this.Payableamt);
    if(this.PaymentType=="cash")
    {
      let loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
      loader.present();
      this.http.get(this.apiurl+'/S1/service.ashx?method=Order&pid='+this.pid+'&uid='+this.uid+'&ActualAmt='+this.cartval+
                    '&AdvAmt='+this.Payableamt+'&PaymentMode='+this.PaymentType+'&AdvType_Half_Full='+this.PayAmtType+
                    '&Qty=1&paymentID=cash'+'&address='+this.address1+'&city='+this.city+'&pin='+this.pin+
                    '&landmark='+this.landmark
      ).subscribe(data1=> {
        loader.dismiss();
        //alert(data1);
      if(data1==null || data1=="")
      {      
        this.presentToast("No Record(s)");
      }
      else
      {
        this.nav.push(ordersuccess, {payid: "Cash"});
      }
    });
    }
    else{
    var options = {
      description: 'Credits towards consultation',
      image: 'https://i.imgur.com/3g7nmJC.png',
      currency: 'INR',
      key: 'rzp_test_1DP5mmOlF5G5ag',
      amount: this.Payableamt+'00',
      name: this.name,
      prefill: {
        email: this.email,
        contact: this.mob,
        name: this.name
      },
      theme: {
        color: '#F37254'
      },
      modal: {
        ondismiss: function() {
          alert('dismissed')
        }
      }
    };

    var successCallback = (payment_id) => {
      // alert('payment_id: ' + payment_id);
      //Navigate to another page using the nav controller
      //this.navCtrl.setRoot(SuccessPage)
      //Inject the necessary controller to the constructor
      let loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
      loader.present();
      this.http.get(this.apiurl+'/S1/service.ashx?method=Order&pid='+this.pid+'&uid='+this.uid+'&ActualAmt='+this.cartval+
                    '&AdvAmt='+this.Payableamt+'&PaymentMode='+this.PaymentType+'&AdvType_Half_Full='+this.PayAmtType+
                    '&Qty=1&paymentID='+payment_id+'&address='+this.address1+'&city='+this.city+'&pin='+this.pin+
                    '&landmark='+this.landmark
      ).subscribe(data1=> {
        loader.dismiss();
        //alert(data1);
      if(data1==null || data1=="")
      {      
        this.presentToast("No Record(s)");
      }
      else
      {
        this.nav.push(ordersuccess, {payid: payment_id});
      }
    });
    };

    var cancelCallback = (error) => {
      alert(error.description + ' (Error ' + error.code + ')');
      //Navigate to another page using the nav controller
      //this.navCtrl.setRoot(ErrorPage)
    };

    RazorpayCheckout.open(options, successCallback, cancelCallback);
  }
}
}

//
