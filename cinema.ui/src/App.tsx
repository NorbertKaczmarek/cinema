import { ChevronLeft, DollarSignIcon, RabbitIcon } from 'lucide-react';

import { Badge, BadgeVariant } from 'Components/Badge';
import { Button, ButtonSize, ButtonVariant } from 'Components/Button';
import { Card } from 'Components/Card';
import { Checkbox } from 'Components/Checkbox';
import { Label } from 'Components/Label';

const App = () => {
  return (
    <div className="flex gap-4 p-2">
      <div className="flex flex-col gap-2">
        Badge
        <Badge>Default</Badge>
        <Badge variant={BadgeVariant.Danger}>Danger</Badge>
        <Badge variant={BadgeVariant.Success}>Success</Badge>
        <Badge variant={BadgeVariant.Disabled}>Disabled</Badge>
        <Badge variant={BadgeVariant.Warning}>Warning</Badge>
        <Badge variant={BadgeVariant.Info}>Info</Badge>
        <Badge variant={BadgeVariant.Outline}>Outline</Badge>
      </div>
      <div className="flex flex-col gap-2">
        Button
        <Button>Default</Button>
        <Button variant={ButtonVariant.Secondary}>Secondary</Button>
        <Button size={ButtonSize.Large} variant={ButtonVariant.Danger}>
          Danger
        </Button>
        <Button size={ButtonSize.Small} variant={ButtonVariant.Success}>
          Success
        </Button>
        <Button variant={ButtonVariant.Warning}>Warning</Button>
        <Button variant={ButtonVariant.Link}>Link</Button>
        <Button variant={ButtonVariant.Outline}>Outline</Button>
        <Button variant={ButtonVariant.Ghost}>Ghost</Button>
        <Button isLoading>Secondary</Button>
        <Button icon={<RabbitIcon />}>With icon</Button>
        <Button
          variant={ButtonVariant.Outline}
          icon={<ChevronLeft />}
          isButtonIcon
          size={ButtonSize.Icon}
        />
      </div>
      <div className="flex flex-col gap-2">
        Card
        <Card
          title="Card Title"
          description="card description"
          classNames={{ wrapper: 'w-64' }}
          footerContent="Footer content"
        >
          Card content
        </Card>
        <Card
          title="$45,543.12"
          header={
            <h4 className="text-md flex items-center justify-between pb-2 font-medium">
              <span>Total Revenue</span> <DollarSignIcon className="h-4 w-4" />
            </h4>
          }
          description="+20.5% from last month"
          classNames={{ wrapper: 'w-72' }}
        />
      </div>
      <div className="flex flex-col gap-2">
        Checkbox
        <Checkbox name="checkbox" label="Accept terms and conditions" />
        <Checkbox name="checkbox2" label="Accept terms and conditions" isDisabled isChecked />
        <Checkbox
          name="checkbox3"
          classNames={{ wrapper: 'items-center' }}
          customLabel={
            <Label htmlFor="Checkbox" className="flex items-center gap-2 text-pink-500">
              Accept terms and conditions
              <RabbitIcon />
            </Label>
          }
        />
      </div>
    </div>
  );
};

export default App;
