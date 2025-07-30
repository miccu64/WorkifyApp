import { Component, EventEmitter, inject, input, Input, Output } from '@angular/core';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { PlanDto } from '../../../dtos/plan.dto';
import { CreateEditPlanFormComponent } from '../../create-edit-plan/create-edit-plan-form.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-plan-card',
  imports: [MatCard, MatCardContent, MatCardTitle],
  templateUrl: './plan-card.component.html',
  styleUrls: ['./plan-card.component.scss']
})
export class PlanCardComponent {
  plan = input<PlanDto>();

  private dialog = inject(MatDialog);

  onPlanClick() {
    this.dialog.open(CreateEditPlanFormComponent, {
      data: { plan: this.plan() }
    });
  }
}
