import { Component, inject, OnInit } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule, MatCardTitle } from '@angular/material/card';
import { PlanDto } from '../../dtos/plan.dto';
import { PlanCardComponent } from '../subcomponents/plan-card/plan-card.component';
import { WorkoutService } from '../../services/workout.service';
import { CreateEditPlanFormComponent } from '../create-edit-plan/create-edit-plan-form.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-plans-list',
  imports: [MatCard, MatCardTitle, MatListModule, MatIconModule, MatButtonModule, MatCardModule, PlanCardComponent],
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
