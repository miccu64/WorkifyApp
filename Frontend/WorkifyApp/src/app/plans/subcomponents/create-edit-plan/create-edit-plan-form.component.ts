import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
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
import {
  NgLabelTemplateDirective,
  NgOptionComponent,
  NgOptionTemplateDirective,
  NgSelectComponent
} from '@ng-select/ng-select';
import { getBodyPartName } from '../../../utils/body-part-helpers';

@Component({
  selector: 'app-create-edit-plan-form',
  templateUrl: './create-edit-plan-form.component.html',
  styleUrls: ['./create-edit-plan-form.component.scss'],
  imports: [
    MatCard,
    MatCardModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatButton,
    MatButtonModule,
    FormsModule,
    NgLabelTemplateDirective,
    NgOptionTemplateDirective,
    NgSelectComponent,
    NgOptionComponent
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CreateEditPlanFormComponent implements OnInit {
  form!: FormGroup;
  exercises: ExerciseDto[] = [];

  searchText = '';
  selectedExercises: ExerciseDto[] = [];

  plan: PlanDto = inject(MAT_DIALOG_DATA)?.plan;
  private workoutService = inject(WorkoutService);
  private formBuilder = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<CreateEditPlanFormComponent>);

  ngOnInit(): void {
    this.exercises = [...this.workoutService.exercises];
    this.selectedExercises = this.exercises.filter(exercise => this.plan?.exercisesIds.includes(exercise.id));

    this.form = this.formBuilder.group({
      name: [this.plan?.name ?? '', Validators.required],
      description: [this.plan?.description ?? ''],
      exercises: [this.selectedExercises, Validators.required]
    });
  }

  customSearchFn(text: string, exercise: ExerciseDto): boolean {
    text = text.toLowerCase();
    return (
      exercise.name.toLowerCase().includes(text) || getBodyPartName(exercise.bodyPart).toLowerCase().includes(text)
    );
  }

  getBodyPartAsString(exercise: ExerciseDto): string {
    return getBodyPartName(exercise.bodyPart);
  }

  async onSubmit(): Promise<void> {
    if (!this.form.valid) {
      return;
    }

    const parameters: CreateEditPlanDto = {
      name: this.form.get('name')?.value,
      description: this.form.get('description')?.value,
      exercisesIds: (this.form.get('exercises')?.value as ExerciseDto[]).map(exercise => exercise.id)
    };

    let request: Observable<number>;
    if (!this.plan) {
      request = this.workoutService.createPlan(parameters);
    } else {
      request = this.workoutService.editPlan(this.plan.id, parameters);
    }

    await firstValueFrom(request);

    await this.workoutService.refreshPlans();

    this.dialogRef.close();
  }
}
