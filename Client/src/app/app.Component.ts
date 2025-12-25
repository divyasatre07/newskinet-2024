import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header.component';
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,HeaderComponent],
  templateUrl: './app.Component.html',
  styleUrl: './app.Component.scss'
})
export class AppComponent implements OnInit {
baseUrl = 'https://localhost:44370/api/';

  private http = inject(HttpClient);
  title='Client'
  products:Product[]=[];

  ngOnInit(): void {
   this.http.get<pagination<Product>>(this.baseUrl + 'products')
.subscribe({
  next: response => this.products=response.data,
  error: err => console.log(err),
  complete:()=> console.log('complete')
});

  }
}
