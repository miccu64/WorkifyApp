import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { BodyPartEnum } from '../../dtos/enums/body-part.enum';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { ListComponent } from '../../layout/list/list.component';
import { WorkoutService } from '../../services/workout.service';
import { getBodyParts } from '../../utils/body-part-helpers';
import { CreateEditExerciseFormComponent } from '../subcomponents/create-edit-exercise/create-edit-exercise-form.component';
import { ExerciseCardComponent } from '../subcomponents/exercise-card/exercise-card.component';

@Component({
  selector: 'app-exercises-list',
  imports: [ExerciseCardComponent, ListComponent, MatInputModule, MatSelectModule],
  templateUrl: './exercises-list.component.html',
  styleUrl: './exercises-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExercisesListComponent implements OnInit {
  exercises: ExerciseDto[] = [];
  kvBodyParts = getBodyParts();
  selectedBodyPart: BodyPartEnum | null = null;

  private allExercises: ExerciseDto[] = [];

  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);
  private changeDetectorRef = inject(ChangeDetectorRef);

  ngOnInit(): void {
    this.refreshData();
  }

  openCreateExerciseForm(): void {
    const dialogRef = this.dialog.open(CreateEditExerciseFormComponent);
    dialogRef.afterClosed().subscribe(() => {
      this.refreshData();
    });
  }

  filterChanged(e: MatSelectChange): void {
    if (e.value === undefined) {
      this.exercises = [...this.allExercises];
    } else {
      const bodyPart: BodyPartEnum = e.value;
      this.exercises = [...this.allExercises.filter(exercise => exercise.bodyPart === bodyPart)];
    }
  }

  private refreshData(): void {
    this.allExercises = [...this.workoutService.exercises];
    this.exercises = [...this.allExercises];
    this.selectedBodyPart = null;

    this.changeDetectorRef.markForCheck();
  }
}
