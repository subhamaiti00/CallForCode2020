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
import { Modal, ModalController, ModalOptions } from 'ionic-angular';
import { ModalPage } from "../pages/modal-page/modal-page";
import { assignorder } from "./assignorder";
@Component({
  selector: 'page-DonorHistory',
  templateUrl: 'DonorHistory.html'
})

export class DonorHistory {
  // search condition
 
  public Orders: any;apiurl:any;catid:any;uid:any;ctname:any;
  public num:any=[{val:''}]
  constructor(private storage: Storage, public nav: NavController, public popoverCtrl: PopoverController,
    public navParams: NavParams,public http: HttpClient,public loadingCtrl: LoadingController,public toastCtrl: ToastController,private modal: ModalController) 
    {
      let loader = this.loadingCtrl.create({
        content: "Please wait..."
      });
      loader.present();
     
      this.storage.get('apiurl').then((val) => {
      this.apiurl=val;
      this.storage.get('uid').then((val) => {
        this.uid=val;
        this.http.get(this.apiurl+'/S1/service.ashx?method=OrderHistoryByDonor&uid='+this.uid).subscribe(data1 => {
            loader.dismiss();
            //alert(data1);
          if(data1==null || data1=="")
          {      
            loader.dismiss();
            //this.presentToast("No Record(s)");
          }
          else
          {
            this.Orders=data1;
          }
        });
      });

  });

  }


 
 
}

//
