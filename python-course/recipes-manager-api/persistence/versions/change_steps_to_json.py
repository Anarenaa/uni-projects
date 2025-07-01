"""change steps to json

Revision ID: change_steps_to_json
Revises: rename_ingridients_table
Create Date: 2025-05-27 15:51:06.000000

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa

# revision identifiers, used by Alembic.
revision: str = 'change_steps_to_json'
down_revision: Union[str, None] = 'rename_ingridients_table'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    """Upgrade schema."""
    # Convert comma-separated string to array
    op.execute("UPDATE recipes SET steps = string_to_array(steps, ',') WHERE steps IS NOT NULL AND steps != ''")
    
    # Change column type to ARRAY
    op.alter_column('recipes', 'steps',
                    existing_type=sa.String(length=255),
                    type_=sa.ARRAY(sa.String(255)),
                    postgresql_using='steps::text[]',
                    existing_nullable=False)


def downgrade() -> None:
    """Downgrade schema."""
    # Convert array back to comma-separated string
    op.execute("UPDATE recipes SET steps = array_to_string(steps, ',') WHERE steps IS NOT NULL")
    
    # Change column type back to String
    op.alter_column('recipes', 'steps',
                    existing_type=sa.ARRAY(sa.String(255)),
                    type_=sa.String(length=255),
                    existing_nullable=False)
