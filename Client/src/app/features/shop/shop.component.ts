import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { MatCard, MatCardModule } from '@angular/material/card';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { DialogRef } from '@angular/cdk/dialog';
import { FilterDialogComponent } from './filter-dialog/filter-dialog.component';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatMenu, MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatListModule, MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { CommonModule } from '@angular/common';
import { ShopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    CommonModule,
    ProductItemComponent,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatListModule,
    MatPaginator,
    FormsModule
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {

  private shopServices = inject(ShopService);
  private dialogService = inject(MatDialog);

  products?: pagination<Product>;

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'price: Low-High', value: 'priceAsc' },
    { name: 'price: High-Low', value: 'priceDesc' },
  ];

  shopParams = new ShopParams();
  PageSizeOption = [5, 10, 15, 20]
  ngOnInit() {
    this.initializeShop();
  }

  initializeShop() {
    this.shopServices.getTypes();
    this.shopServices.getBrands();
    this.getProducts();
  }
  getProducts() {
    this.shopServices.getProduct(this.shopParams).subscribe({

      next: response => this.products = response,
      error: error => console.log(error)
    });
  }
onSearchChange(){
  this.shopParams.pageNumber=1;
  this.getProducts;
}

  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOptions = event.options[0];
    if (selectedOptions) {
      this.shopParams.sort = selectedOptions.value;
      this.shopParams.pageNumber = 1;
      console.log(this.shopParams.sort);
      this.getProducts();
    }

  }
  openFilterDialog() {
    const dialogRef = this.dialogService.open(FilterDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types

      }
    });
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          console.log(result);
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.pageNumber = 1;
          this.getProducts();


        }
      }
    })
  }
}
