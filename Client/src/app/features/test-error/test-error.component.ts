import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss',
})
export class TestErrorComponent {

  private http = inject(HttpClient);
  baseUrl = 'https://localhost:44370/api/';
  validationErrors: string[] = [];

  get500Error() {
  this.http.get(this.baseUrl + 'buggy/internalerror').subscribe();
}

  get404Error() {
    this.http.get(this.baseUrl + 'buggy/notfound').subscribe();
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe();
  }

  get401Error() {
    this.http.get(this.baseUrl + 'buggy/unauthorized').subscribe();
  }

  get400ValidationError() {
    this.http.post(this.baseUrl + 'buggy/validationerror', {}).subscribe({
      next : response => console.log(response),
      error : error => this.validationErrors = error
    });
  }
}
