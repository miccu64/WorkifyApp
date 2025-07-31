import { Component, inject, OnInit } from '@angular/core';
import { PlanDto } from '../../dtos/plan.dto';
import { ActivatedRoute } from '@angular/router';
import { WorkoutService } from '../../services/workout.service';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { ListComponent } from '../../layout/list/list.component';
import { ExerciseCardComponent } from '../../exercises/subcomponents/exercise-card/exercise-card.component';
import { InfoTableComponent } from '../../layout/info-table/info-table.component';
import { MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { CreateEditPlanFormComponent } from '../subcomponents/create-edit-plan/create-edit-plan-form.component';

@Component({
  selector: 'app-plan-preview',
  imports: [
    ListComponent,
    ExerciseCardComponent,
    InfoTableComponent,
    MatIcon,
    MatIconModule,
    MatFabButton,
    MatButtonModule
  ],
  templateUrl: './plan-preview.component.html',
  styleUrls: ['./plan-preview.component.scss']
})
export class PlanPreviewComponent implements OnInit {
  plan!: PlanDto;
  exercises!: ExerciseDto[];
  tableData!: Map<string, string>;

  private activatedRoute = inject(ActivatedRoute);
  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);

  ngOnInit(): void {
    const planId = Number(this.activatedRoute.snapshot.paramMap.get('planId'));
    const plan = this.workoutService.plans.find(p => p.id == planId);
    if (plan) {
      this.plan = plan;
      this.exercises = this.workoutService.exercises.filter(e => plan.exercisesIds.includes(e.id));

      this.tableData = new Map<string, string>([
        ['Name', this.plan.name],
        ['Description', this.plan.description ?? '-'],
        ['Exercises', this.plan.exercisesIds.length.toString()]
      ]);
    } else {
      throw new Error('Plan not found', { cause: planId });
    }
  }

  editPlan(): void {
    const dialogRef = this.dialog.open(CreateEditPlanFormComponent);
    dialogRef.afterClosed().subscribe((refreshedPlans: PlanDto[]) => {
      if (refreshedPlans) {
        this.workoutService.plans = refreshedPlans;
      }
    });
  }

  deletePlan(): void {}
}
