import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { OaComponent } from './components/oa.component';
import { OaRoutingModule } from './oa-routing.module';

@NgModule({
  declarations: [OaComponent],
  imports: [CoreModule, ThemeSharedModule, OaRoutingModule],
  exports: [OaComponent],
})
export class OaModule {
  static forChild(): ModuleWithProviders<OaModule> {
    return {
      ngModule: OaModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<OaModule> {
    return new LazyModuleFactory(OaModule.forChild());
  }
}
