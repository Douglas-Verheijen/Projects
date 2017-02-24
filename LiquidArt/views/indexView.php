<div class="page-content">
	<div class="page-info-wrapper">
		<div class="page-info">
			<div class="page-info-heading">Liquid Art</div>
			<div class="page-info-content">
				<?php foreach ($model as $artist) : ?>
					<a href="artist.php?artist=<?php echo $artist->Id; ?>">
						<div class="indexImageComponent">
							<img class="indexImageComponentImage" src="<?php echo $artist->Thumbnail; ?>"></image>
							<div class="indexImageComponentHighlight"></div>
							<div class="indexImageComponentLabel"><?php echo $artist->Name; ?></div>
						</div>
					</a>
				<?php endforeach ?>
			</div>
		</div>
    </div>
</div>