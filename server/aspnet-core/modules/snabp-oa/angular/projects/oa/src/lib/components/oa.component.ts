import { Component, OnInit } from '@angular/core';
import { OaService } from '../services/oa.service';

@Component({
  selector: 'lib-oa',
  template: ` <p>oa works!</p> `,
  styles: [],
})
export class OaComponent implements OnInit {
  constructor(private service: OaService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
