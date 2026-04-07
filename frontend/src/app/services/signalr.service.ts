import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/stockHub')
      .build();
  }

  startConnection() {
    this.hubConnection.start()
      .then(() => console.log('SignalR connected'))
      .catch(err => console.error('SignalR error: ', err));
  }

  addStockUpdatedListener(callback: (data: any) => void) {
    this.hubConnection.on('StockUpdated', callback);
  }
}
