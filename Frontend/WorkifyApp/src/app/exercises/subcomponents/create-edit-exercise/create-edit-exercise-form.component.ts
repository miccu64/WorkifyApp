import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { firstValueFrom, Observable } from 'rxjs';
import { ExerciseDto } from '../../../dtos/exercise.dto';
import { WorkoutService } from '../../../services/workout.service';
import { BodyPartEnum } from '../../../dtos/enums/body-part.enum';
import { CreateEditExerciseDto } from '../../../dtos/parameters/create-edit-exercise.dto';

@Component({
  selector: 'app-create-edit-exercise-form',
  templateUrl: './create-edit-exercise-form.component.html',
  styleUrls: ['./create-edit-exercise-form.component.scss'],
  imports: [MatCard, MatCardModule, MatInputModule, MatSelectModule, ReactiveFormsModule, MatButton, MatButtonModule]
})
export class CreateEditExerciseFormComponent implements OnInit {
  form!: FormGroup;
  kvBodyParts = Object.entries(BodyPartEnum).filter(keyValue => Number.isInteger(keyValue[1]));

  exercise: ExerciseDto = inject(MAT_DIALOG_DATA)?.exercise;

  private workoutService = inject(WorkoutService);
  private formBuilder = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<CreateEditExerciseFormComponent>);

  ngOnInit() {
    this.form = this.formBuilder.group({
      name: [this.exercise?.name, Validators.required],
      bodyPart: [this.exercise?.bodyPart, Validators.required],
      description: [this.exercise?.description]
    });
  }

  async onSubmit() {
    if (!this.form.valid) {
      return;
    }

    const parameters: CreateEditExerciseDto = {
      name: this.form.get('name')?.value,
      bodyPart: this.form.get('bodyPart')?.value as BodyPartEnum,
      description: this.form.get('description')?.value
    };

    let request: Observable<number>;
    if (this.exercise == null) {
      request = this.workoutService.createExercise(parameters);
    } else {
      request = this.workoutService.editExercise(this.exercise.id, parameters);
    }

    await firstValueFrom(request);

    await this.workoutService.refreshPlansAndExercises();

    this.dialogRef.close();
  }
}
