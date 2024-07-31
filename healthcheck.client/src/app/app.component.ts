import { Component } from '@angular/core';
import { ConnectionService, ConnectionServiceOptions, ConnectionState } from 'ng-connection-service';
import { map, Observable, Subscription, tap } from 'rxjs';
import { environment } from '../environments/environment';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent  {
  title = 'HealthCheck';

  //public isOffline: Observable<boolean>;
  status!: string;
  currentState!: ConnectionState;
  subscription = new Subscription();

  constructor(private connectionService: ConnectionService) {
   
        //this.isOffline = this.connectionService.monitor(options)
     // .pipe(map(state => !state.hasNetworkConnection || state.hasInternetAccess));
  }

  ngOnInit(): void {
    const options: ConnectionServiceOptions = {
      enableHeartbeat: true,
      heartbeatUrl: environment.baseUrl + 'api/heartbeat',
      heartbeatInterval: 10000
    };
    this.subscription.add(
      this.connectionService.monitor(options).pipe(
        tap((newState: ConnectionState) => {
          this.currentState = newState;

          if (this.currentState.hasNetworkConnection && this.currentState.hasInternetAccess) {
            this.status = 'ONLINE';
          
          } else {
            this.status = 'OFFLINE';
            
          }
          console.log('status', this.status);
        })
      ).subscribe()
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
