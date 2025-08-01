import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/Services/dataService/data.service';
import { OrdersService } from 'src/app/Services/orderService/orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent {

  subsription!: Subscription;
    token: string | null = null;
    orders:any=[]

    constructor(
        private dataService: DataService,
        private ordersService:OrdersService
      ){}
      ngOnInit(): void {
        this.subsription = this.dataService.AccessToken.subscribe(
          (result) => (this.token = result)
        );
        this.getOrders()
      }
      getOrders(){
        this.ordersService.getAllOrders().subscribe(
          (response) => {
            this.orders = response.data;
          },
          (error) => {
            console.error('Error fetching orderslist items');
          }
        )
      }
}
