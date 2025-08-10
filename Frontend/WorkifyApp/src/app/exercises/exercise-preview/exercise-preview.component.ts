import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkoutService } from '../../services/workout.service';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { ListComponent } from '../../layout/list/list.component';
import { InfoTableComponent } from '../../layout/info-table/info-table.component';
import { MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { CreateEditExerciseFormComponent } from '../subcomponents/create-edit-exercise/create-edit-exercise-form.component';
import { BodyPartEnum } from '../../dtos/enums/body-part.enum';
import { CommonModule } from '@angular/common';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-exercise-preview',
  imports: [ListComponent, InfoTableComponent, MatIcon, MatIconModule, MatFabButton, MatButtonModule, CommonModule],
  templateUrl: './exercise-preview.component.html',
  styleUrls: ['./exercise-preview.component.scss']
})
export class ExercisePreviewComponent implements OnInit {
  exercise!: ExerciseDto;
  tableData!: Map<string, string>;

  disabledBtnText = this.exercise?.isCustom === false ? '' : 'This exercise is predefined and cannot be modified';

  private activatedRoute = inject(ActivatedRoute);
  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  ngOnInit(): void {
    const exerciseId = Number(this.activatedRoute.snapshot.paramMap.get('exerciseId'));
    this.refreshData(exerciseId);
  }

  editExercise(): void {
    const dialogRef = this.dialog.open(CreateEditExerciseFormComponent, { data: { exercise: this.exercise } });
    dialogRef.afterClosed().subscribe(() => {
      this.refreshData(this.exercise.id);
    });
  }

  async deleteExercise(): Promise<void> {
    if (confirm('Are you sure to delete exercise "' + this.exercise.name + '"?')) {
      await firstValueFrom(this.workoutService.deleteExercise(this.exercise.id));

      await this.workoutService.refreshPlansAndExercises();

      await this.router.navigate(['/app/exercises/list']);
    }
  }

  private refreshData(exerciseId: number): void {
    const exercise = this.workoutService.exercises.find(e => e.id === exerciseId);
    if (!exercise) {
      throw new Error('Exercise not found');
    }

    this.exercise = exercise;
    this.tableData = new Map<string, string>([
      ['Body part', BodyPartEnum[exercise.bodyPart]],
      ['Description', exercise.description ?? '-']
    ]);
  }
}
