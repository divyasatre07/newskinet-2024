import { bootstrapApplication } from '@angular/platform-browser';

import { provideHttpClient } from '@angular/common/http';
import { AppComponent } from './app/app.Component';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient()
  ]
});
