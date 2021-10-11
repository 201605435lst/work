import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OaComponent } from './oa.component';

describe('OaComponent', () => {
  let component: OaComponent;
  let fixture: ComponentFixture<OaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
