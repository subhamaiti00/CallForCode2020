import { Component, ViewChild } from "@angular/core";
import { Platform, Nav, Events } from "ionic-angular";

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Keyboard } from '@ionic-native/keyboard';

import { HomePage } from "../pages/home/home";
import { LoginPage } from "../pages/login/login";
import { LocalWeatherPage } from "../pages/local-weather/local-weather";
import { myorderPage } from "../pages/orders/myorder";
import { viewcartPage } from "../pages/viewcart/viewcart";
import { vieworder } from "../pages/orders/vieworder";
import { neworders } from "../pages/Admin/neworders";
import { registerSE } from "../pages/Admin/registerSE";
import { assignedorder } from "../pages/Admin/assignedorder";
import { sehome } from "../pages/SE/sehome";
import { ConsumerHome } from "../pages/home/consumerhome";
import { DonorHome } from "../pages/home/donorhome";
import { Storage } from '@ionic/storage';
import { verifycon } from "../pages/SE/verifycon";
import { SEHistory } from "../pages/SE/SEHistory";
import { chatbot } from "../pages/home/chatbot";
import { DonorHistory } from "../pages/Admin/DonorHistory";
export interface MenuItem {
    title: string;
    component: any;
    icon: string;
}

@Component({
  templateUrl: 'app.html'
})

export class MyApp {
  @ViewChild(Nav) nav: Nav;

  rootPage: any = LoginPage;

  appMenuItems: Array<MenuItem>;
  name:any;rolefull:any;
  constructor(
    public platform: Platform,
    public statusBar: StatusBar,
    public splashScreen: SplashScreen,
    public keyboard: Keyboard,public events: Events,public storage: Storage
  ) {
    this.initializeApp();
    events.subscribe('loggedin', (role, time,name) => {
      if(role=="c"){
        this.rolefull="Consumer";
    this.appMenuItems = [
      {title: 'Home', component: ConsumerHome, icon: 'home'},
      {title: 'My Cart', component: viewcartPage, icon: 'cart'},
      {title: 'My Requests', component: vieworder, icon: 'basket'},
      {title: 'Assistance', component: chatbot, icon: 'basket'}
    ];
  }
  else if(role=="d"){
    this.rolefull="Donor";
    this.appMenuItems = [
      {title: 'Home', component: DonorHome, icon: 'home'},
      {title: 'Open Requests', component: neworders, icon: 'cart'},
     // {title: 'Completed Orders', component: vieworder, icon: 'basket'},
     // {title: 'Payment Report', component: vieworder, icon: 'basket'},
      {title: 'My Donations', component:DonorHistory, icon: 'contacts'}
    ];
  }
  else{
    this.rolefull="Volunteer";
    this.appMenuItems = [
      {title: 'My Tasks', component: sehome, icon: 'home'},
      
      {title: 'History', component: SEHistory, icon: 'clipboard'},
      {title: 'Verify Consumer', component: verifycon, icon: 'body'}
     
    ];

  }
    this.name=name;
    this.rolefull
  });
  }

  initializeApp() {
    this.platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.

      //*** Control Splash Screen
      // this.splashScreen.show();
      // this.splashScreen.hide();

      //*** Control Status Bar
      this.statusBar.styleDefault();
      this.statusBar.overlaysWebView(false);

      //*** Control Keyboard
      //this.keyboard.disableScroll(true);
    });
  }

  openPage(page) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
    this.nav.setRoot(page.component);
  }

  logout() {
    this.storage.clear();
    this.nav.setRoot(LoginPage);
  }
  editprofile()
  {
    
  }
}
