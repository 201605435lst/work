import { ModuleWithProviders, NgModule } from '@angular/core';
import { MY_PROJECT_NAME_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class OaConfigModule {
  static forRoot(): ModuleWithProviders<OaConfigModule> {
    return {
      ngModule: OaConfigModule,
      providers: [MY_PROJECT_NAME_ROUTE_PROVIDERS],
    };
  }
}
