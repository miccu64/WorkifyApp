import { Component, inject, input } from '@angular/core';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { Router } from '@angular/router';
import { PlanDto } from '../../../dtos/plan.dto';

@Component({
  selector: 'app-plan-card',
  imports: [MatCard, MatCardContent, MatCardTitle],
  templateUrl: './plan-card.component.html',
  styleUrls: ['./plan-card.component.scss']
})
export class PlanCardComponent {
  readonly plan = input<PlanDto>();

  private router = inject(Router);

  planPreview(): void {
    this.router.navigate([`app/plans/preview/${this.plan()?.id}`]);
  }
}
