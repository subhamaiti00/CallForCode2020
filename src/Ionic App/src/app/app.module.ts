import {NgModule} from "@angular/core";
import {IonicApp, IonicModule} from "ionic-angular";
import {BrowserModule} from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import {IonicStorageModule} from '@ionic/storage';

import {StatusBar} from '@ionic-native/status-bar';
import {SplashScreen} from '@ionic-native/splash-screen';
import {Keyboard} from '@ionic-native/keyboard';

import {ActivityService} from "../services/activity-service";
import {TripService} from "../services/trip-service";
import {WeatherProvider} from "../services/weather";

import {MyApp} from "./app.component";

import {SettingsPage} from "../pages/settings/settings";
import {CheckoutTripPage} from "../pages/checkout-trip/checkout-trip";
import {HomePage} from "../pages/home/home";
import {LoginPage} from "../pages/login/login";
import {NotificationsPage} from "../pages/notifications/notifications";
import {RegisterPage} from "../pages/register/register";
import {SearchLocationPage} from "../pages/search-location/search-location";
import {TripDetailPage} from "../pages/trip-detail/trip-detail";
import {TripsPage} from "../pages/trips/trips";
import {LocalWeatherPage} from "../pages/local-weather/local-weather";
import { myorderPage } from "../pages/orders/myorder";
import { ProductsPage } from "../pages/products/products";
import { viewcartPage } from "../pages/viewcart/viewcart";
import { placeorderPage } from "../pages/orders/placeorder";
import { ordersuccess } from "../pages/orders/ordersuccess";
import { failorder } from "../pages/orders/failorder";
import { vieworder } from "../pages/orders/vieworder";
import { subcatPage } from "../pages/home/subcat";
import { neworders } from "../pages/Admin/neworders";
import { assignorder } from "../pages/Admin/assignorder";
import { registerSE } from "../pages/Admin/registerSE";
import { assignedorder } from "../pages/Admin/assignedorder";
import { sehome } from "../pages/SE/sehome";
import { vieworderSE } from "../pages/SE/vieworderSE";
import { HomeRegisterPage } from "../pages/homeregister/homeregister";
import { ConsumerHome } from "../pages/home/consumerhome";
import { DonorHome } from "../pages/home/donorhome";
import { verifycon } from "../pages/SE/verifycon";
import { SEHistory } from "../pages/SE/SEHistory";
import { chatbot } from "../pages/home/chatbot";
import { DonorHistory } from "../pages/Admin/DonorHistory";


// import services
// end import services
// end import services

// import pages
// end import pages

@NgModule({
  declarations: [
    MyApp,
    SettingsPage,
    CheckoutTripPage,
    HomePage,
    LoginPage,
    LocalWeatherPage,
    NotificationsPage,
    RegisterPage,
    SearchLocationPage,
    TripDetailPage,
    TripsPage,myorderPage,ProductsPage,viewcartPage,placeorderPage,ordersuccess,vieworder,subcatPage,
    neworders,assignorder,registerSE,assignedorder,sehome,vieworderSE,HomeRegisterPage,ConsumerHome,
    DonorHome,verifycon,SEHistory,chatbot,DonorHistory
    
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    IonicModule.forRoot(MyApp, {
      scrollPadding: false,
      scrollAssist: true,
      autoFocusAssist: false
    }),
    IonicStorageModule.forRoot({
      name: '__ionic3_start_theme',
        driverOrder: ['indexeddb', 'sqlite', 'websql']
    })
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    SettingsPage,
    CheckoutTripPage,
    HomePage,
    LoginPage,
    LocalWeatherPage,
    NotificationsPage,
    RegisterPage,
    SearchLocationPage,
    TripDetailPage,
    TripsPage,
    myorderPage,ProductsPage,viewcartPage,placeorderPage,ordersuccess,vieworder,subcatPage,
    neworders,assignorder,registerSE,assignedorder,sehome,vieworderSE,HomeRegisterPage,ConsumerHome,
    DonorHome,verifycon,SEHistory,chatbot,DonorHistory
    
  ],
  providers: [
    StatusBar,
    SplashScreen,
    Keyboard,
    ActivityService,
    TripService,
    WeatherProvider
  ]
})

export class AppModule {
}
