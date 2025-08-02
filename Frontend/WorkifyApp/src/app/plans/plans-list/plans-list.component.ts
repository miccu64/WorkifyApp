import { Component, inject, OnInit } from '@angular/core';
import { PlanDto } from '../../dtos/plan.dto';
import { PlanCardComponent } from '../subcomponents/plan-card/plan-card.component';
import { WorkoutService } from '../../services/workout.service';
import { CreateEditPlanFormComponent } from '../subcomponents/create-edit-plan/create-edit-plan-form.component';
import { MatDialog } from '@angular/material/dialog';
import { ListComponent } from '../../layout/list/list.component';

@Component({
  selector: 'app-plans-list',
  imports: [ListComponent, PlanCardComponent],
  templateUrl: './plans-list.component.html',
  styleUrl: './plans-list.component.scss'
})
export class PlansListComponent implements OnInit {
  plans: PlanDto[] = [];

  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);

  ngOnInit(): void {
    this.plans = this.workoutService.plans;
  }

  openCreateForm(): void {
    const dialogRef = this.dialog.open(CreateEditPlanFormComponent);
    dialogRef.afterClosed().subscribe(() => {
      this.plans = this.workoutService.plans;
    });
  }
}
