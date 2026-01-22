import { inject, Injectable } from '@angular/core';
import { pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ShopParams } from '../../shared/models/shopParams';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:44370/api/';
  
    private http = inject(HttpClient);
    types:string[]=[];
    brands:string[]=[];
getProducts(shopParams: ShopParams) {
  let params = new HttpParams();

  if (shopParams.brands.length > 0) {
    params = params.append('brands', shopParams.brands.join(','));
  }

  if (shopParams.types.length > 0) {
    params = params.append('types', shopParams.types.join(','));
  }

  if (shopParams.sort) {
    params = params.append('sort', shopParams.sort);
  }

  if (shopParams.search) {
    params = params.append('search', shopParams.search);
  }

  params = params.append('pageSize', shopParams.pageSize);
  params = params.append('pageIndex', shopParams.pageNumber);

  return this.http.get<pagination<Product>>(
    this.baseUrl + 'products',
    { params }
  );
}

getProductById(id: number) {
  return this.http.get<Product>(
    this.baseUrl + 'products/' + id
  );
}




getBrands() {
  this.http.get(this.baseUrl + 'products/brands', {
    responseType: 'text'
  }).subscribe({
    next: response => {
      this.brands = JSON.parse(response);
    }
  });
}

getTypes() {
  this.http.get(this.baseUrl + 'products/types', {
    responseType: 'text'
  }).subscribe({
    next: response => {
      this.types = JSON.parse(response);
    }
  });
}
}