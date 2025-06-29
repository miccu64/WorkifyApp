import { Component, inject, OnInit } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute } from '@angular/router';
import { PlanDto } from '../../dtos/plan.dto';

@Component({
  selector: 'app-plans-list',
  imports: [MatToolbarModule, MatListModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './plans-list.component.html',
  styleUrl: './plans-list.component.scss'
})
export class PlansListComponent implements OnInit {
  private route = inject(ActivatedRoute);

  plans: PlanDto[] = [];

  ngOnInit(): void {
    this.plans = this.route.snapshot.data['plans'];
  }
}
