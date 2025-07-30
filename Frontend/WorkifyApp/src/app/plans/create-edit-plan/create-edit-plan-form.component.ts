import { Component, Input, Output, EventEmitter, OnInit, inject, input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateEditPlanDto } from '../../dtos/parameters/create-edit-plan.dto';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule } from '@angular/forms';
import { WorkoutService } from '../../services/workout.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PlanDto } from '../../dtos/plan.dto';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { firstValueFrom, Observable } from 'rxjs';

@Component({
  selector: 'app-create-edit-plan-form',
  templateUrl: './create-edit-plan-form.component.html',
  styleUrls: ['./create-edit-plan-form.component.scss'],
  imports: [MatCard, MatCardModule, MatInputModule, MatSelectModule, ReactiveFormsModule, MatButton, MatButtonModule]
})
export class CreateEditPlanFormComponent implements OnInit {
  form!: FormGroup;
  exercises: ExerciseDto[] = [];

  public plan: PlanDto = inject(MAT_DIALOG_DATA)?.plan;
  private workoutService = inject(WorkoutService);
  private formBuilder = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<CreateEditPlanFormComponent>);

  ngOnInit() {
    this.form = this.formBuilder.group({
      name: [this.plan?.name ?? '', Validators.required],
      description: [this.plan?.description ?? ''],
      exercisesIds: [this.plan?.exercisesIds ?? [], Validators.required]
    });
    this.exercises = this.workoutService.exercises;
  }

  async onSubmit() {
    if (!this.form.valid) {
      return;
    }

    const parameters: CreateEditPlanDto = {
      name: this.form.get('name')?.value,
      description: this.form.get('description')?.value,
      exercisesIds: this.form.get('exercisesIds')?.value
    };

    let request: Observable<number>;
    if (this.plan == null) {
      request = this.workoutService.createPlan(parameters);
    } else {
      // TODO: edit
      request = this.workoutService.createPlan(parameters);
    }

    await firstValueFrom(request);
    this.workoutService.plans = await firstValueFrom(this.workoutService.getPlans());

    this.dialogRef.close();
  }
}
