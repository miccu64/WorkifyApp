import { Component } from '@angular/core';
import { MatCard, MatCardActions, MatCardContent, MatCardSubtitle, MatCardTitle } from '@angular/material/card';

@Component({
  selector: 'app-plans-list',
  imports: [MatCard, MatCardTitle, MatCardSubtitle, MatCardActions, MatCardContent],
  templateUrl: './plans-list.component.html',
  styleUrl: './plans-list.component.scss'
})
export class PlansListComponent {
  plans = [
    { name: 'Basic', price: '$10/mo', features: ['Feature A', 'Feature B'] },
    { name: 'Pro', price: '$25/mo', features: ['Feature A', 'Feature B', 'Feature C'] },
    { name: 'Enterprise', price: 'Contact us', features: ['All features'] }
  ];
}
