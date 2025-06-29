import { Component, inject, OnInit } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule, MatCardTitle } from '@angular/material/card';
import { ActivatedRoute } from '@angular/router';
import { PlanDto } from '../../dtos/plan.dto';
import { PlanCardComponent } from '../subcomponents/plan-card/plan-card.component';

@Component({
  selector: 'app-plans-list',
  imports: [MatCard, MatCardTitle, MatListModule, MatIconModule, MatButtonModule, MatCardModule, PlanCardComponent],
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
