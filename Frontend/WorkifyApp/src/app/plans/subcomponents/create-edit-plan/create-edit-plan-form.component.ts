import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { firstValueFrom, Observable } from 'rxjs';
import { ExerciseDto } from '../../../dtos/exercise.dto';
import { CreateEditPlanDto } from '../../../dtos/parameters/create-edit-plan.dto';
import { PlanDto } from '../../../dtos/plan.dto';
import { WorkoutService } from '../../../services/workout.service';

@Component({
  selector: 'app-create-edit-plan-form',
  templateUrl: './create-edit-plan-form.component.html',
  styleUrls: ['./create-edit-plan-form.component.scss'],
  imports: [MatCard, MatCardModule, MatInputModule, MatSelectModule, ReactiveFormsModule, MatButton, MatButtonModule]
})
export class CreateEditPlanFormComponent implements OnInit {
  form!: FormGroup;
  exercises: ExerciseDto[] = [];

  plan: PlanDto = inject(MAT_DIALOG_DATA)?.plan;
  private workoutService = inject(WorkoutService);
  private formBuilder = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<CreateEditPlanFormComponent>);

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      name: [this.plan?.name ?? '', Validators.required],
      description: [this.plan?.description ?? ''],
      exercisesIds: [this.plan?.exercisesIds ?? [], Validators.required]
    });
    this.exercises = this.workoutService.exercises;
  }

  async onSubmit(): Promise<void> {
    if (!this.form.valid) {
      return;
    }

    const parameters: CreateEditPlanDto = {
      name: this.form.get('name')?.value,
      description: this.form.get('description')?.value,
      exercisesIds: this.form.get('exercisesIds')?.value
    };

    let request: Observable<number>;
    if (this.plan === null) {
      request = this.workoutService.createPlan(parameters);
    } else {
      request = this.workoutService.editPlan(this.plan.id, parameters);
    }

    await firstValueFrom(request);

    await this.workoutService.refreshPlans();

    this.dialogRef.close();
  }
}
