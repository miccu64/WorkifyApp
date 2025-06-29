import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { PlanDto } from '../../../dtos/plan.dto';

@Component({
  selector: 'app-plan-card',
  imports: [MatCard, MatCardContent, MatCardTitle],
  templateUrl: './plan-card.component.html',
  styleUrls: ['./plan-card.component.scss']
})
export class PlanCardComponent {
  @Input() plan!: PlanDto;
  // @Output() click = new EventEmitter<void>();

  onPlanClick() {
    //this.click.emit();
  }
}
