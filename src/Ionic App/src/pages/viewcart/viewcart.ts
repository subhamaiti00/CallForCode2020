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
import { Modal, ModalController, ModalOptions } from 'ionic-angular';
import { ModalPage } from "../pages/modal-page/modal-page";
@Component({
  selector: 'page-viewcart',
  templateUrl: 'viewcart.html'
})

export class viewcartPage {
  // search condition
  
  public Products: any;apiurl:any;catid:any;uid:any;cartval:number;data:any;visible:any;  
  public RecomProducts:Array<Object> = [];
  constructor(private storage: Storage, public nav: NavController, public popoverCtrl: PopoverController,
    public navParams: NavParams,public http: HttpClient,public loadingCtrl: LoadingController,public toastCtrl: ToastController,private modal: ModalController) 
    {
     
      
      this.catid=this.navParams.get('id');
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
    this.visible=true;
    let loader = this.loadingCtrl.create({
      content: "Please wait..."
    });
    loader.present();
    this.http.get('https://callforcodeapi-impressive-wolf-lg.eu-gb.mybluemix.net/ViewCart?uid='+this.uid).subscribe(data1=> {
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
  this.http.get('https://callforcodeapi-impressive-wolf-lg.eu-gb.mybluemix.net/api/Products/ProductRecom?uid='+this.uid).subscribe(data2=> {
      loader.dismiss();
      //alert(data1);
    if(data2==null || data2=="")
    {      
     // this.presentToast("No Record(s)");
    }
    else
    {
      this.data=data2;  
      this.RecomProducts=[];
      var cnt=1;
      for (let item of this.data){
        if(cnt<=5)
        {
        this.RecomProducts.push(item);        
        }
        else
        {
          break;
        }
        cnt++;
      }
     this.visible=false;
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
  Order() {
    //this.nav.setRoot(viewcartPage);
    this.nav.push(placeorderPage, {From: "Cart",Pid:0});
    }
remove(id) {
      //this.nav.push(ProductsPage, {id: id});
      let loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
      loader.present();
      this.http.get(this.apiurl+'/S1/service.ashx?method=removefromcart&uid='+this.uid+'&cartid='+id).subscribe(data1 => {
        loader.dismiss();
        //alert(data1);
      if(data1==null || data1=="")
      {      
        this.presentToast("Invalid Data/Email Exists");
      }
      else if(data1=="1")
      {
        this.Loaddata();
      let toast = this.toastCtrl.create({
        message: 'Removed from cart.',
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
    fnAddToCart(id) {
      //this.nav.push(ProductsPage, {id: id});
      let loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
      this.http.get(this.apiurl+'/S1/service.ashx?method=addtocart&uid='+this.uid+'&pid='+id+'&qty=1').subscribe(data1 => {
        loader.dismiss();
        //alert(data1);
      if(data1==null || data1=="")
      {      
        //this.presentToast("Invalid Data/Email Exists");
      }
      else if(data1=="1")
      {
      this.Loaddata();
      let toast = this.toastCtrl.create({
        message: 'Added.',
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
  fnMinusFromCart(id)
  {
    
  }
  openModal(name,desc,img) {

    const myModalOptions: ModalOptions = {
      enableBackdropDismiss: false
    };

    const myModalData = {
      name: name,
      sdesc: desc,
      pimg:img,
      apiurl:this.apiurl
    };

    const myModal: Modal = this.modal.create('ModalPage', { data: myModalData }, myModalOptions);
    myModal.present();

    myModal.onDidDismiss((data) => {
      console.log("I have dismissed.");
      console.log(data);
    });

    myModal.onWillDismiss((data) => {
      console.log("I'm about to dismiss");
      console.log(data);
    });

  }
}

//
