import { Component, Input } from '@angular/core';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { ExerciseDto } from '../../../dtos/exercise.dto';

@Component({
  selector: 'app-exercise-card',
  imports: [MatCard, MatCardContent, MatCardTitle],
  templateUrl: './exercise-card.component.html',
  styleUrls: ['./exercise-card.component.scss']
})
export class ExerciseCardComponent {
  @Input() exercise!: ExerciseDto;
  // @Output() click = new EventEmitter<void>();

  onExerciseClick() {
    //this.click.emit();
  }
}
